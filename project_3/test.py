import numpy as np
import cv2

cap = cv2.VideoCapture('ball_1.mov')


while (cap.isOpened()):
	# take first frame of the video
	ret,frame = cap.read()

	cv2.imshow("fra", frame)
	#cv2.waitKey()

	cimg = np.copy(frame)
	frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

	circles = cv2.HoughCircles(frame, cv2.cv.CV_HOUGH_GRADIENT, 2, 1000,
	                            param1=40, param2=20, minRadius=30, maxRadius=60)
	cimg.fill(0)
	circles = np.uint16(np.around(circles))
	for i in circles[0]:
	    # draw the outer circle
	    cv2.circle(cimg,(i[0],i[1]),i[2],(0,255,0),2)
	    # draw the center of the circle
	    cv2.circle(cimg,(i[0],i[1]),2,(0,0,255),3)

	cv2.imshow('detected circles', cimg)
	#cv2.waitKey()
	
	print circles
	if cv2.waitKey(1) & 0xFF == ord('q'):
		break


cv2.destroyAllWindows()
cap.release()