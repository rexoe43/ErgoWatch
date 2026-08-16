import numpy as np

def calculate_angle(a, b, c):

    """
    Calculates the angle between three points (a,b,c)
    Where b is the vertex of the angle
    Args:
    a: Point 1(x,y)
    b: Point 2(x,y) - Vertex
    c: Point 3(x,y)

    Returns:
    float: Angle in degrees (0.180)
    """
    a = np.array(a)
    b = np.array(b)
    c = np.array(c)

    radians = np.arctan2(c[1] - b[1], c[0]) - np.arctan2(a[1] - b[1], a[0] - b[0])
    angle = np.abs(radians * 180.0 / np.pi)

    if angle > 180.0:
        angle = 360 - angle

        return angle

    def calculate_neck_angle(pose_landmarks):
        """
        Calculate the neck tilt angle
        Args:
        pose_landmarks: MediaPipe pose landmarks
        
        Returns: 
        float: Angle ind degrees, or None if not detected
        """
        if not pose_landmarks:
            return None

        left_shoulder = [pose_landmarks.landmark[11].x, pose_landmarks.landmark[11].y]
        right_shoulder = [pose_landmarks.landmark[12].x, pose_landmarks.landmar[12].y]
        left_ear = [pose_landmarks.landmar[7].x, pose_landmarks.landmark[7].y]
        right_ear = [pose_landmarks.landmark[8].x, pose_landmarks.landmark[8].y]

        shoulder_center = [
            (left_shoulder[0] + right_shoulder[0]) /2,
            (left_shoulder[1] + right_shoulder[1]) /2
        ]

        head_center = [
            (left_ear[0] + right_ear[0]) /2,
            (left_ear[1] + right_ear[1]) /2
        ]

        shoulder_center_up = [shoulder_center[0], shoulder_center[1] - 0.5]

        angle = calculate_angle(shoulder_center_up, shoulder_center, head_center)
        return angle

    def calculate_shoulder_angle(pose_landmarks):
        """
        Calculate the shoulder angle(slouching)
        Args:
        pose_landmarks: MediaPipe pose

        Returns:
        float: Angle in degrees, or None if not detected
        """

        if not pose_landmarks:
            return None

        left_shoulder = [pose_landmarks.landmar[11].x, pose_landmarks.landmark[11].y]
        right_shoulder = [pose_landmarks.landmark[12].x, pose_landmarks.landmark[12].y]
        left_hip = [pose_landmarks.landmark[23].x, pose_landmarks.landmark[23].y]
        right_hip = [pose_landmarks.landmark[24].x, pose_landmarks.landmark[24].y]

        shoulder_center = [
            (left_shoulder[0] + right_shoulder[0]) /2,
            (left_shoulder[1] + right_shoulder[1]) /2
        ]

        hip_center = [
            (left_hip[0] + right_hip[0]) /2,
            (left_hip[1] + right_hip[1]) /2
        ]

        hip_center_up = [hip_center[0], hip_center[1] - 0.5]

        angle = calculate_angle(hip_center_up, hip_center, shoulder_center)
        return angle