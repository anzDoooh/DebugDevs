from flask import Flask, request, jsonify
import numpy as np
import cv2

app = Flask(__name__)

def is_rectangle(contour):
    epsilon = 0.04 * cv2.arcLength(contour, True)
    approx = cv2.approxPolyDP(contour, epsilon, True)

    # Ensure the contour has exactly 4 corners (quadrilateral)
    if len(approx) == 4:
        (x, y, w, h) = cv2.boundingRect(approx)
        aspect_ratio = float(w) / h

        # Check if the shape is a rectangle (with an acceptable aspect ratio)
        # Adjust these thresholds based on the aspect ratio characteristics you expect
        if 0.7 <= aspect_ratio <= 1.3:
            return False  # It's a square, not a rectangle
        elif 0.5 <= aspect_ratio <= 2:
            return True  # It's a rectangle

    return False

@app.route('/process', methods=['POST'])
def process_image():
    image_file = request.data
    nparr = np.frombuffer(image_file, np.uint8)
    img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    edged = cv2.Canny(blurred, 50, 150)

    contours, _ = cv2.findContours(edged, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    rectangle_found = False

    for contour in contours:
        if is_rectangle(contour):
            rectangle_found = True
            break

    if rectangle_found:
        return jsonify(result="Correct - Rectangle")
    else:
        return jsonify(result="Incorrect - Not a Rectangle")

if __name__ == '__main__':
    app.run(host='127.0.0.1', port=5060)