Suvamsh Shivaprasad
Michelle Lawrence
Nick Walther

The 3D scene can be created from two stereo images (left and right) using the following command:

./stereo.py [output_point_cloud] [left_image] [right_image] [focal_length] [window size] [min disparity] [uniqueness ratio]

We opted to use an implementation of the StereoSGBM function that we found which takes multiple parameters rather than the simpler, two-parameter function, hoping to give us an image with less noise. More parameters gave us more tools to 'tweak' the stereo image to produce a cleaner result. 

Unfortunately more parameters, in addition to the simple fact that every pair of images will have unique features, scales, etc. means that the function we built was essentially personalized for our main test image. So, we generalized the disparity map function in our personal testing function to pass parameters that would optimize each stereo pair we were dealing with. 

Through a simple process of trial and error, we found that the window size, the minimum disparity, and the uniqueness ratio yielded the most noticeable cleaning results between different images. Finding the optimum values, however, proved a computationally intensive task, and we were unable to do so for each pair of our personal test images. With more time or perhaps access to the interactive GUI for parameter manipulation (found here: http://blog.martinperis.com/2011/08/opencv-stereo-matching.html), we could have better computed values to match each image pair. 