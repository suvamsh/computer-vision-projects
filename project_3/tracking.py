"""Project 3: Tracking.

In this project, you'll track objects in videos.
"""

import cv2
import math
import numpy as np


def get_initial_position(frame, hough_parameters):
    """Finds the initial position of the ball.

    Arguments:
      frame: a single video frame containing the ball you want identified
      hough_parameters: the parameters to pass to the Hough circle detector

    Outputs:
      returns the first (best-fit) circle found by the Hough cicles algorithm
    """

    # convert frame to grayscale
    frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # run the Hough circle detector given the caller's parameters
    circles = cv2.HoughCircles(
        frame,
        cv2.cv.CV_HOUGH_GRADIENT,
        **hough_parameters)

    # unravel the return list and return the first circle
    circles = np.uint16(np.around(circles))
    return circles[0][0]


def track_ball(video, hough_parameters, initial_radius_scalar=1):
    """Track the ball's center in 'video'.

    Arguments:
      video: an open cv2.VideoCapture object containing a video of a ball
        to be tracked.
      hough_parameters: the parameters to pass to the Hough circle detector
      initial_radius_scalar: the radius scalar to apply to the initial
        circle found by Hough circle.

    Outputs:
      a list of (min_x, min_y, max_x, max_y) four-tuples containing the pixel
      coordinates of the rectangular bounding box of the ball in each frame.
    """

    # take first frame of the video
    ret, frame = video.read()
    height, width, depth = frame.shape

    # get initial location of ball
    initial_circle = get_initial_position(frame, hough_parameters)

    # setup return structure
    coords = []

    # setup initial location of window
    r, h, c, w = int(
        initial_circle[0] - initial_circle[2]), int(
        initial_circle[2] * 2 * initial_radius_scalar), \
        initial_circle[1] - initial_circle[2], int(
            initial_circle[2] * 2 * initial_radius_scalar)

    # setup initial track window and add it to the return list
    track_window = (c, r, w, h)
    coords.append(
        (initial_circle[0] -
         initial_circle[2],
         initial_circle[1] -
         initial_circle[2],
         initial_circle[0] +
         initial_circle[2],
         initial_circle[1] +
         initial_circle[2]))

    # set up the ROI for tracking
    # credit: http://docs.opencv.org/trunk/doc/py_tutorials/py_video/
    #   py_meanshift/py_meanshift.html
    roi = frame[r:r + h, c:c + w]
    hsv_roi = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
    mask = cv2.inRange(
        hsv_roi, np.array(
            (0., 60., 32.)), np.array(
            (180., 255., 255.)))
    roi_hist = cv2.calcHist([hsv_roi], [0], mask, [180], [0, 180])
    cv2.normalize(roi_hist, roi_hist, 0, 255, cv2.NORM_MINMAX)

    # setup the termination criteria, either 20 iterations or move atleast 1 pt
    term_crit = (cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 30, 1)

    # start iterating through the video
    while(1):
        # get frame
        ret, frame = video.read()

        if ret:
            # convert frame to HSV and get tracking mask
            hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)
            dst = cv2.calcBackProject([hsv], [0], roi_hist, [0, 180], 1)

            # apply meanshift to get the new location
            ret, track_window = cv2.meanShift(dst, track_window, term_crit)

            # append new window to coordinates
            x, y, w, h = track_window
            coords.append((x, y, x + w, y + h))
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

    hough_parameters = {
        'dp': 7,
        'minDist': 1000,
        'param1': 40,
        'param2': 20,
        'minRadius': 0,
        'maxRadius': 0
    }

    return track_ball(video, hough_parameters)


def track_ball_2(video):
    """As track_ball_1, but for ball_2.mov."""

    hough_parameters = {
        'dp': 7,
        'minDist': 1000,
        'param1': 40,
        'param2': 20,
        'minRadius': 0,
        'maxRadius': 0
    }

    return track_ball(video, hough_parameters)


def track_ball_3(video):
    """As track_ball_1, but for ball_2.mov."""

    hough_parameters = {
        'dp': 2,
        'minDist': 1000,
        'param1': 40,
        'param2': 40,
        'minRadius': 0,
        'maxRadius': 0
    }

    return track_ball(video, hough_parameters, 0.9)


def track_ball_4(video):
    """As track_ball_1, but for ball_2.mov."""

    hough_parameters = {
        'dp': 7,
        'minDist': 1000,
        'param1': 40,
        'param2': 20,
        'minRadius': 0,
        'maxRadius': 0
    }

    return track_ball(video, hough_parameters)


def track_face(video):
    """As track_ball_1, but for face.mov."""
    #load the face detection dataset which has already been trained
    faceCascade = cv2.CascadeClassifier(
        'data/haarcascade_frontalface_default.xml')
    outList = []
    while True:
        # pull return value and frame
        #return value -> whether frame exists or not
        #frame -> actual frame object
        ret, frame = video.read()
        if not ret:
            break
        #convert frame to grayscale, known to be better for face detection
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        # Use cascade classifier to detect face
        faces = faceCascade.detectMultiScale(gray,
                                             scaleFactor=1.1,
                                             minNeighbors=5,
                                             minSize=(30, 30),
                                             flags = cv2.cv.CV_HAAR_SCALE_IMAGE
                                             )
        #add rectangle coordinates of tracked face
        outList.append(
            (faces[0][0],
             faces[0][1],
                faces[0][0] +
                faces[0][2],
                faces[0][1] +
                faces[0][3]))
    return outList
