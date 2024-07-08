from flask import Flask, request, jsonify
import cv2
import numpy as np

app = Flask(__name__)

def preprocess_image(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    _, thresholded = cv2.threshold(blurred, 60, 255, cv2.THRESH_BINARY_INV)
    return thresholded

def is_triangle_drawn(image):
    processed = preprocess_image(image)
    edged = cv2.Canny(processed, 30, 150)

    contours, _ = cv2.findContours(edged, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    for cnt in contours:
        perimeter = cv2.arcLength(cnt, True)
        approx = cv2.approxPolyDP(cnt, 0.04 * perimeter, True)

        # Check if the contour is a triangle (specifically 3 vertices)
        if len(approx) == 3:
            return True

    return False

@app.route('/process', methods=['POST'])
def process_triangle():
    file = request.data
    npimg = np.frombuffer(file, np.uint8)
    image = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    if is_triangle_drawn(image):
        result = "Correct! A triangle is drawn on the paper."
    else:
        result = "Wrong! No triangle is drawn on the paper."

    return jsonify(result=result)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=6060)