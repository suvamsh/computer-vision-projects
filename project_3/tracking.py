"""Project 3: Tracking.

In this project, you'll track objects in videos.
"""

import cv2
import math
import numpy as np


def get_initial_position(frame):
    frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    circles = cv2.HoughCircles(frame, cv2.cv.CV_HOUGH_GRADIENT, 2, 1000,
                                param1=40, param2=20, minRadius=20, maxRadius=34)
    circles = np.uint16(np.around(circles))
    print circles
    return circles[0][0]


def track_ball(video):

    # data structure to keep track of all frames
    frame_history = []

    # take first frame of the video
    ret,frame = video.read()
    height, width, depth = frame.shape
    #frame_history.append(cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY))

    # setup initial pixel history for background subtraction
    current_frame = 0
    '''pixel_set = [][][]
    for i in range(0, width):
        for j in range(0, height):
            pixel_set[i][j][current_frame] = frame[i][j] 
            '''
    # get initial location of ball
    initial_circle = get_initial_position(frame)

    # setup return structure
    coords = []

    # setup initial location of window
    r,h,c,w = initial_circle[0] - initial_circle[2], initial_circle[2] * 2, initial_circle[1] - initial_circle[2], initial_circle[2] * 2
    track_window = (c,r,w,h)
    coords.append((initial_circle[0] - initial_circle[2], initial_circle[1] - initial_circle[2], initial_circle[0] + initial_circle[2], initial_circle[1] + initial_circle[2]))
    print coords

    # set up the ROI for tracking
    roi = frame[r:r+h, c:c+w]
    hsv_roi =  cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    mask = cv2.inRange(hsv_roi, np.array((0., 60.,32.)), np.array((180.,255.,255.)))
    roi_hist = cv2.calcHist([hsv_roi],[0],mask,[180],[0,180])
    cv2.normalize(roi_hist,roi_hist,0,255,cv2.NORM_MINMAX)

    # Setup the termination criteria, either 20 iterations or move by atleast 1 pt
    term_crit = (cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 20, 1)

    while(1):
        ret ,frame = video.read()
   #     frame_history.append(cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY))

        if ret == True:
            # figure out new background
            '''background = frame.copy()
            pixel_set = []
            for i in range(0, width):
                for j in range(0, height):
                    for (f in frame_history):
                        pixel_set
'''



            hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
            dst = cv2.calcBackProject([hsv],[0],roi_hist,[0,180],1)

            # Apply meanshift to get the new location
            ret, track_window = cv2.meanShift(dst, track_window, term_crit)

            # Append new window to coordinates
            x,y,w,h = track_window
            coords.append((x, y, x+w, y+h))
        else:
            break

    return coords
 

def track_ball_1(video):
    """Track the ball's center in 'video'.

    Arguments:
      video: an open cv2.VideoCapture object containing a video of a ball
        to be tracked.

    Outputs:
      a list of (min_x, min_y, max_x, max_y) four-tuples containing the pixel
      coordinates of the rectangular bounding box of the ball in each frame.
    """

    return track_ball(video)
        
    

def track_ball_2(video):
    """As track_ball_1, but for ball_2.mov."""
    return track_ball(video)
    


def track_ball_3(video):
    """As track_ball_1, but for ball_2.mov."""
    return track_ball(video)


def track_ball_4(video):
    """As track_ball_1, but for ball_2.mov."""
    return track_ball(video)

def track_face(video):
    """As track_ball_1, but for face.mov."""
    pass
