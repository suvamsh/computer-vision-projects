import cv2

img4 = cv2.imread("test_data/books_2.png")
img5 = cv2.imread("test_data/books_3.png")
img1 = cv2.imread("my_panos/img1.jpg")
img2 = cv2.imread("my_panos/img2.jpg")
img3 = cv2.imread("my_panos/img3.jpg")
import pano_stitcher
imgs = [img1, img3, img2]
#pano_stitcher.create_mosaic(imgs, imgs)
i,c = pano_stitcher.warp_image(img1, pano_stitcher.homography(img1,img2))

cv2.imwrite("warp_test.png", i)