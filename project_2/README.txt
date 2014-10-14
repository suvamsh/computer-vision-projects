Suvamsh Shivaprasad
Michelle Lawrence
Nick Walther

The 3D scene can be created from two stereo images (left and right) using the following command:

./stereo.py [output_point_cloud] [left_image] [right_image] [focal_length] [window size] [min disparity] [uniqueness ratio]

We used the StereoSGBM function which takes multiple parameters rather than the simpler two parametered function, hoping to give us an image with less noise. Unfortunately, there are many parameters that need to be tweaked and optimizing for these parameters is a computationally intensive task. We were unable to find the optimum parameters to fit our test images. Nevertheless, our implementation passes the PEP8 style as well as test cases provided.