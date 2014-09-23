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

    # find features and descriptors
    kp1, des1 = sift.detectAndCompute(image_b, None)
    kp2, des2 = sift.detectAndCompute(image_a, None)
    bf = cv2.BFMatcher()
    matches = bf.knnMatch(des1, des2, k=2)

    # store good matches
    best_matches = []
    for x, y in matches:
        if x.distance < 0.75 * y.distance:
            best_matches.append(x)
    src_pts = np.float32([kp1[x.queryIdx].pt for x in best_matches]).reshape(-1,1,2)
    dst_pts = np.float32([kp2[y.trainIdx].pt for y in best_matches]).reshape(-1,1,2)
    # print cv2.perspectiveTransform(dst_pts, src_pts)
    M, mask = cv2.findHomography(src_pts, dst_pts, cv2.RANSAC, 5.0)
    return M

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
    rows, cols, _ = image.shape
    # rows *= int(homography[0][0])
    # cols *= int(homography[1][1])
    arr = np.matrix(
        [[1.0, 0.0, homography[0][2]], [0.0, 1.0, homography[1][2]], [0.0, 0.0, 1.0]])
    #print arr, "\n"
    #print homography
    t1, t2 = homography[0][2], homography[1][2]
    # homography2 = arr * homography
    #homography[0][2] = 0
    #homography[1][2] = 0
    #print homography
    homography = arr * homography
    img = cv2.warpPerspective(image, homography, (int(cols*homography[0][0]), int(rows*homography[1][1])))
    img = cv2.cvtColor(img, cv2.COLOR_BGR2BGRA)
    return img, (t1, t2)

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
    minx = 0
    maxx = 0
    miny = 0
    maxy = 0
    whichimagex = 0
    whichimagey = 0
    for i in origins:
        if i[0] < minx:
            minx = i[0]
            whichimage = origins.index(i)
        elif i[0] > maxx:
            maxx = i[0]
            whichimage = origins.index(i)

        if i[1] < miny:
            miny = i[1]
            whichimagey = origins.index(i)
        elif i[1] > maxy:
            maxy = i[1]
            whichimagey = origins.index(i)
    h, w, _ = images[whichimagex].shape
    width = abs(minx) + maxx + w
    h, w, _ = images[whichimagey].shape
    height = abs(miny) + maxy + h
    toskip = 0
    ret_image = np.zeros((height, width, 4), np.uint8)
    for k in range(len(images)):
        if origins[k] == (0, 0):
            toskip = k
            continue
        h, w, _ = images[k].shape
        for y in range(h):
            for x in range(w):
                tmp = images[k][y][x]
                if images[k][y][x][3] == 255:
                    dy = origins[k][1] + abs(miny) + y
                    dx = origins[k][0] + abs(minx) + x
                    ret_image[dy][dx] = tmp

    h, w, _ = images[toskip].shape
    for y in range(h):
        for x in range(w):
            tmp = images[toskip][y][x]
            if images[toskip][y][x][3] == 255:
                    dy = origins[toskip][1] + abs(miny) + y
                    dx = origins[toskip][0] + abs(minx) + x
                    ret_image[dy][dx] = tmp
    return ret_image
    pass
