"""Project 1: Panorama stitching.

In this project, you'll stitch together images to form a panorama.

A shell of starter functions that already have tests is listed below.

TODO: Implement!
"""

import cv2 
import numpy as np


def homography(image_a, image_b):
    """Returns the homography mapping image_b into alignment with image_a.

    Arguments:
      image_a: A grayscale input image.
      image_b: A second input image that overlaps with image_a.

    Returns: the 3x3 perspective transformation matrix (aka homography)
             mapping points in image_b to corresponding points in image_a.
    """
    sift = cv2.SIFT()
    #find features and descriptors 
    kp1, des1 = sift.detectAndCompute(image_a, None)
    kp2, des2 = sift.detectAndCompute(image_b, None)
    
    #brute force matcher
    bf = cv2.BFMatcher(cv2.NORM_L2)

    # Draw first 10 matches.
    matches = bf.knnMatch(des1, des2, k = 10)
    best_matches = []
    for x in matches:
      #print x.distance, y.distance
      #if x.distance <0.7*y.distance:
      print x
      best_matches.append(x)

    src_pts = np.float32([ kp1[x.queryIdx].pt for x in best_matches ])
    dst_pts = np.float32([ kp2[y.trainIdx].pt for y in best_matches ])

    #for x in best_matches:
    #    print kp1[x.queryIdx].pt, kp2[x.trainIdx].pt
    M, mask = cv2.findHomography(src_pts, dst_pts, cv2.RANSAC,5.0)
    return M
    #matches = sorted(matches, key = lambda x:x.distance)

    #img = cv2.drawMatches(image_a, kp1, image_b, kp2, matches[:10], flags=2)
    #retval, mask = cv2.findHomography()
    #img1 = cv2.drawKeypoints(image_a, kp1)
    #img2 = cv2.drawKeypoints(image_b, kp2)
    #cv2.imwrite('image_a.jpg',img1)
    #cv2.imwrite('matches.jpg',img)
    pass


def warp_image(image, homography):
    """Warps 'image' by 'homography'

    Arguments:
      image: a 3-channel image to be warped.
      homography: a 3x3 perspective projection matrix mapping points
                  in the frame of 'image' to a target frame.

    Returns:
      - a new 4-channel image containing the warped input, resized to contain
        the new image's bounds. Translation is offset so the image fits exactly
        within the bounds of the image. The fourth channel is an alpha channel
        which is zero anywhere that the warped input image does not map in the
        output, i.e. empty pixels.
      - an (x, y) tuple containing location of the warped image's upper-left
        corner in the target space of 'homography', which accounts for any
        offset translation component of the homography.
    """
    pass


def create_mosaic(images, origins):
    """Combine multiple images into a mosaic.

    Arguments:
      images: a list of 4-channel images to combine in the mosaic.
      origins: a list of the locations upper-left corner of each image in
               a common frame, e.g. the frame of a central image.

    Returns: a new 4-channel mosaic combining all of the input images. pixels
             in the mosaic not covered by any input image should have their
             alpha channel set to zero.
    """
    pass
