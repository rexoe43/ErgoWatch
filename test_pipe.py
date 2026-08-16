import json
import time
import win32pipe
import win32file
import pywin32_system32

def send_alert(alert_data):
    try:
        handle = win32file.CreateFile(
           r'\\.\pipe\ergonomics_pipe',
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0, None, win32file.OPEN_EXISTING, 0, None
        )

        data = json.dumps(alert_data) + '\n'
        win32file.WriteFile(handle, data.encode('utf-8'))
        win32file.CloseHandle(handle)
        print("Alert send correctly")

    except Exception as e:
        print(f"Error: {e}")

alert = {
    "timestamp": "2026-08-15T12:00:00",
    "type": "posture",
    "severity": "high",
    "message": "Bas posture detected. Neckle angle: 22°",
    "details": {
        "neck_angle": 22.5,
        "shoulder_angle": 18.3,
        "blink_rate": 8.0,
        "duration_seconds": 0,
        "message": "Your neck is 22° forward"
    }
}

if __name__ == "__main__":
    print("Sending the test alerts to ErgoWatch...")
    print("Press Ctrl+C to stop")

    while True:
        send_alert(alert)
        time.sleep(10)