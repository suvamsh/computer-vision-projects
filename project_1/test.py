import cv2
import pano_stitcher


img4 = cv2.imread("test_data/books_2.png")
img5 = cv2.imread("test_data/books_3.png")
img1 = cv2.imread("my_panos/img1.jpg")
img2 = cv2.imread("my_panos/img2.jpg", -1)
img3 = cv2.imread("my_panos/img3.jpg")
imgs = [img1, img2]
read_alpha = -1

i1,c1 = pano_stitcher.warp_image(img1, pano_stitcher.homography(img2,img1))
i3,c3 = pano_stitcher.warp_image(img3, pano_stitcher.homography(img2,img3))
print i3.shape

cv2.imwrite("my_panos/warp_img1_test.png", i1)
cv2.imwrite("my_panos/warp_img2_test.png", img2)
cv2.imwrite("my_panos/warp_img3_test.png", i3)



#read warped images in
img_1_warped = cv2.imread("my_panos/warp_img1_test.png", read_alpha)
img_2 = cv2.imread("my_panos/warp_img2_test.png")
img_2 = cv2.cvtColor(img_2, cv2.COLOR_BGR2BGRA)
img_3_warped = cv2.imread("test_data/books_3_warped.png", read_alpha)
print img_3_warped.shape

# Create the panorama mosaic.
images = (img_1_warped, img_3_warped,  img_2)
origins = ((-876, -107), (683, 2), (0,0))
pano = pano_stitcher.create_mosaic(images, origins)

cv2.imwrite("my_panos/our_stitched_image.png", pano)
