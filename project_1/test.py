import cv2
import pano_stitcher

print "Loading images..."
# read warped images in
img_1_warped = cv2.imread("my_panos/warp_img1_test.png", -1)
img_2 = cv2.imread("my_panos/warp_img2_test.png")
img_2 = cv2.cvtColor(img_2, cv2.COLOR_BGR2BGRA)
img_3_warped = cv2.imread("my_panos/warp_img3_test.png", -1)


# Create the panorama mosaic.
images = (img_1_warped, img_3_warped, img_2)
origins = ((-876, -107), (683, 2), (0, 0))
pano = pano_stitcher.create_mosaic(images, origins)

print "Writing panaroma to my_panos/our_stitched_image.png"
cv2.imwrite("my_panos/our_stitched_image.png", pano)
