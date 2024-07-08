from flask import Flask, request, jsonify
import wave
import json
from vosk import Model, KaldiRecognizer

app = Flask(__name__)

# Update with your model path
model_path = r"c:\Users\DELL\Desktop\vosk-model-small-en-us-0.15"
model = Model(model_path)
rec = KaldiRecognizer(model, 16000)

@app.route('/recognize', methods=['POST'])
def recognize():
    if 'file' not in request.files:
        return jsonify({'error': 'No file part'})
    file = request.files['file']
    try:
        wf = wave.open(file, "rb")
        if wf.getnchannels() != 1:
            return jsonify({'error': 'Audio file should be mono channel'})
        rec.AcceptWaveform(wf.readframes(wf.getnframes()))
        result = json.loads(rec.Result())
        return jsonify(result)
    except Exception as e:
        return jsonify({'error': str(e)})

@app.route('/evaluate', methods=['POST'])
def evaluate():
    audio_data = request.files['file'].read()
    word = request.form['word']
    score, hint = evaluate_pronunciation(audio_data, word)
    return jsonify({'score': score, 'hint': hint})

def evaluate_pronunciation(audio_data, word):
    # Dummy implementation
    # Replace with actual pronunciation evaluation logic
    score = 0.8  # Example score
    hint = "Try to stress the first syllable."
    return score, hint

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)