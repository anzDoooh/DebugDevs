from flask import Flask, request, jsonify
import cv2
import numpy as np

app = Flask(__name__)


def is_square_drawn(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    edged = cv2.Canny(blurred, 50, 150)
    contours, _ = cv2.findContours(edged, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    for cnt in contours:
        perimeter = cv2.arcLength(cnt, True)
        approx = cv2.approxPolyDP(cnt, 0.04 * perimeter, True)

        if len(approx) == 4:
            x, y, w, h = cv2.boundingRect(approx)
            aspect_ratio = w / float(h)

            # Ensure the contour is not too small (adjust threshold as needed)
            if cv2.contourArea(approx) > 1500:  # Adjust the area threshold
                # Ensure the aspect ratio is close to 1 (square)
                if 0.95 <= aspect_ratio <= 1.05:  # Adjust aspect ratio range
                    return True

    return False


@app.route('/process', methods=['POST'])
def process_square():
    file = request.data
    npimg = np.frombuffer(file, np.uint8)
    image = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    if is_square_drawn(image):
        result = "Correct! A square is drawn on the paper."
    else:
        result = "Wrong! No square is drawn on the paper."

    return jsonify(result=result)


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8080)
