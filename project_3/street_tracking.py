import numpy as np
import cv2
import sys

# HOGdescriptor


def inside(r, q):
    rx, ry, rw, rh = r
    qx, qy, qw, qh = q
    return rx > qx and ry > qy and rx + rw < qx + qw and ry + rh < qy + qh


def draw_detections(img, rects, thickness=1):
    for x, y, w, h in rects:
        pad_w, pad_h = int(0.15 * w), int(0.05 * h)
        cv2.rectangle(img, (x +
                            pad_w, y +
                            pad_h), (x +
                                     w -
                                     pad_w, y +
                                     h -
                                     pad_h), (0, 255, 0), thickness)


def main(argv):
    args = str(sys.argv[1])
    hogParams = {'hitThreshold': -.5, 'scale': 1.05}
    hog = cv2.HOGDescriptor()
    hog.setSVMDetector(cv2.HOGDescriptor_getDefaultPeopleDetector())

    video = cv2.VideoCapture(args)

    ret, frame = video.read()

    while(ret):

        cimg = np.copy(frame)
        people, w = hog.detectMultiScale(frame, **hogParams)
        filetered = []
        for ri, r in enumerate(people):
            for qi, q in enumerate(people):
                if ri != qi and inside(r, q):
                    print "break"
                    break
            else:
                filetered.append(r)
        # draw_detections(frame, people)
        draw_detections(cimg, filetered, 1)
        cv2.imshow('detected people', cimg)
        cv2.waitKey(2)

        ret, frame = video.read()

    cv2.destroyAllWindows()
    video.release()

if __name__ == "__main__":
    main(sys.argv[1:])
