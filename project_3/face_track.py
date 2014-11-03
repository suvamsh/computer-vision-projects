import numpy as np
import cv2
outList = []
minX = 0
minY = 0
maxX = 0
maxY = 0
outliers = 0
cap = cv2.VideoCapture('test_data/face.mov')
# Create the haar cascade
faceCascade = cv2.CascadeClassifier('data/haarcascade_frontalface_default.xml')
count = 0
while (cap.isOpened()):
	# take first frame of the video
	ret,frame = cap.read()
	if ret == False:
		break
	#cv2.imshow("face",frame)
	#gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
	# Detect faces in the image
	faces = faceCascade.detectMultiScale(frame,
    									scaleFactor=1.1,
    									minNeighbors=5,
    									minSize=(30, 30),
    									flags = cv2.cv.CV_HAAR_SCALE_IMAGE
										)
	# Draw a rectangle around the faces
	for (x, y, w, h) in faces:
		count = count + 1
		if minX - (x-w) > 18 :
			outliers = outliers + 1
			#print (x-w, y-h, x+w, y+h)
		else:
			if count != 1:	
				outList.append((x, y, x + w, y + h))#(x-w, y-h, x+w, y+h))
		minX = x-w
		cv2.rectangle(frame, (x, y), (x+w, y+h), (0, 255, 0), 2)

	# Display the resulting frame
	cv2.imshow('Video', frame)
	if cv2.waitKey(1) & 0xFF == ord('q'):
		break

# When everything is done, release the capture
cap.release()
cv2.destroyAllWindows()
print "count = ", count
print "len of outList = ", len(outList)
print "outliers = ", outliers
