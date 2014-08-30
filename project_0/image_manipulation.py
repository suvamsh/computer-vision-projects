"""Assignment 1: Image Manipulation with OpenCV.

In this assignment, you will implement a few basic image
manipulation tasks using the OpenCV library.

Use the unit tests is image_manipulation_test.py to guide
your implementation, adding functions as needed until all
unit tests pass.
"""

# TODO: Implement!

import cv2


def flip_image(image, horizontal, vertical):
    if horizontal and vertical:
        return cv2.flip(image, -1)
    elif horizontal and not vertical:
        return cv2.flip(image, 1)
    elif not horizontal and vertical:
        return cv2.flip(image, 0)
    return image


def negate_image(nyc):
    return ~nyc


def swap_red_and_green(nyc):
    image = nyc
    for i in range(len(nyc)):
        for j in range(len(nyc[i])):
            image[i][j][0] = nyc[i][j][1]
            image[i][j][1] = nyc[i][j][0]
            image[i][j][2] = nyc[i][j][2]
    return image
