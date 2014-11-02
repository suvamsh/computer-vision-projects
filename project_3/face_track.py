import numpy as np
import cv2

cap = cv2.VideoCapture('test_data/face.mov')
# Create the haar cascade
faceCascade = cv2.CascadeClassifier('data/haarcascade_frontalface_default.xml')
count = 0
while (cap.isOpened()):
	count = count + 1
	# take first frame of the video
	ret,frame = cap.read()
	if ret == False:
		break
	#cv2.imshow("face",frame)
	#gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
	# Detect faces in the image
	faces = faceCascade.detectMultiScale(frame,
    									scaleFactor=1.3,
    									minNeighbors=3,
    									minSize=(30, 30),
    									flags = cv2.cv.CV_HAAR_SCALE_IMAGE
										)
	# Draw a rectangle around the faces
	for (x, y, w, h) in faces:
		#print x-w, y-h, x+w, y+h
		cv2.rectangle(frame, (x, y), (x+w, y+h), (0, 255, 0), 2)

	# Display the resulting frame
	cv2.imshow('Video', frame)
	if cv2.waitKey(1) & 0xFF == ord('q'):
		break

# When everything is done, release the capture
cap.release()
cv2.destroyAllWindows()
print count
