from flask import Flask, request, jsonify
import cv2
import numpy as np

app = Flask(__name__)

def is_circle(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    circles = cv2.HoughCircles(blurred, cv2.HOUGH_GRADIENT, 1.2, 100)

    return circles is not None

@app.route('/process', methods=['POST'])
def process_image():
    file = request.data
    npimg = np.frombuffer(file, np.uint8)
    image = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    if is_circle(image):
        result = "Correct! This is a circle."
    else:
        result = "Wrong! Please try again."

    return jsonify(result=result)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
