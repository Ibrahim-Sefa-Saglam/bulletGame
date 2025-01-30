there is a GameObject "bullet" prefab 
there is a float "rate" 
This script belongs to an  object named "clip"


creata script that:

creates instances of bullets on the right side of the clip, creates each of them in a line at 1/rate time, each bullet is "bulletDistance" away from each other only on x axis, the initial bullet should be  bulletDistance" away from the clip's collider

add the created bullets into a list to keep track of them, 
move each bullet to right for "bulletDistance" over 1/rate time

keep each action/check in methods