import numpy as np
import cv2
import cv
import sys

# def inside(r, q): 
# 	rx, ry, rw, rh = r
# 	qx, qy, qw, qh = q 
# 	return rx > qx and ry > qy and rx + rw < qx + qw and ry + rh < qy + qh


# def draw_detections(img, rects, thickness = 1): 
# 	print "rect"
# 	print rects
# 	# for x, y, w, h in rects:
# 	# 	pad_w, pad_h = int(0.15*w), int(0.05*h)
#  #
#      	cv2.rectangle(img, (x+pad_w, y+pad_h), (x+w-pad_w, y+h-pad_h), (0, 255, 0), thickness)
video = cv2.VideoCapture('ewap_dataset/seq_eth/seq_eth.avi')

bodyCascade = cv2.CascadeClassifier('data/haarcascade_fullbody.xml')

outList = []
while True:
    # pull return value and frame
    # return value -> whether frame exists or not
    # frame -> actual frame object
    ret, frame = video.read()
    if not ret:
        break
    # convert frame to grayscale, known to be better for face detection
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    # Use cascade classifier to detect face
    bodys = bodyCascade.detectMultiScale(gray,
                                         scaleFactor=1.1,
                                         minNeighbors=5,
                                         minSize=(30, 30),
                                         flags = cv2.cv.CV_HAAR_SCALE_IMAGE
                                         )
    
print "found"
print bodys

# hog = cv2.HOGDescriptor()
# hog.setSVMDetector(cv2.HOGDescriptor_getDefaultPeopleDetector())
# hogParams = {'winStride': (8, 8), 'padding': (32, 32), 'scale': 1.05}

# video = cv2.VideoCapture('ewap_dataset/seq_eth/seq_eth.avi')

# s, img = video.read()

# while s and video.isOpened():
	
# 	found, w = hog.detectMultiScale(img, winStride = (8,8), padding = (32,32), scale = 1.05, group_threshold=2)
# 	print found
# 	# found_filter = []
# 	# for ri, r in enumerate(found):
# 	# 	print "new"
# 	# 	print ri
# 	# 	print r
# 	# 	for qi, q in enumerate(found):
# 	# 		print qi 
# 	# 		print q
# 	# 		if ri != qi:
# 	# 			break
# 	# 		if inside(r,q):
# 	# 			break
# 	# 	else:
# 	# 		found_filter.append(r)
# 	# 		print "r"
# 	# 		print r
# 	# draw_detections(img, found)
# 	# draw_detections(img, found_filter,1)

# print "found results"
