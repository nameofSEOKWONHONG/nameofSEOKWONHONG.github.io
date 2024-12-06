window.ecgInterop = {
    ecgLoad: function () {
        const canvas = document.getElementById('ecgCanvas');
        const ctx = canvas.getContext('2d');
        const width = canvas.width;
        const height = canvas.height;

        let data = []; // Stores ECG data
        let x = 0; // Current x-coordinate of the end point
        let timeElapsed = 0; // Elapsed time in milliseconds

        // ECG waveform parameters
        const pWave = { amplitude: 15, duration: 0.08, offset: -0.2 }; // P wave parameters
        const qrsComplex = { amplitude: 50, duration: 0.12, offset: 0 }; // QRS complex parameters
        const tWave = { amplitude: 20, duration: 0.16, offset: 0.4 }; // T wave parameters


        // Generate a pseudo-realistic ECG waveform
        function generateECG(x, isFlat) {
            if (isFlat) {
                return height / 2; // Flatline during cardiac arrest
            }

            // Time within a single heartbeat (normalized to 1 second per cycle)
            const beatDuration = 1.0; // 1 second per heartbeat
            const time = (x / 60) % beatDuration;

            // Generate waveform components
            const p = pWave.amplitude * Math.exp(-Math.pow((time - pWave.offset) / pWave.duration, 2));
            const qrs = qrsComplex.amplitude * Math.exp(-Math.pow((time - qrsComplex.offset) / qrsComplex.duration, 2));
            const t = tWave.amplitude * Math.exp(-Math.pow((time - tWave.offset) / tWave.duration, 2));

            // Combine the waveform components
            return height / 2 - (p + qrs + t);
        }

        function draw() {
            // Determine if the current time is within the "flatline" period
            const isFlat = Math.floor((timeElapsed / 60000) * 12) % 12 < 1; // 5초 평탄화 (1분마다)

            // Clear canvas
            ctx.clearRect(0, 0, width, height);

            // Draw background grid
            ctx.strokeStyle = '#333';
            ctx.lineWidth = 0.5;
            for (let i = 0; i < width; i += 20) {
                ctx.beginPath();
                ctx.moveTo(i, 0);
                ctx.lineTo(i, height);
                ctx.stroke();
            }
            for (let i = 0; i < height; i += 20) {
                ctx.beginPath();
                ctx.moveTo(0, i);
                ctx.lineTo(width, i);
                ctx.stroke();
            }

            // Draw ECG waveform
            ctx.strokeStyle = '#0f0';
            ctx.lineWidth = 2;
            ctx.beginPath();

            if (data.length > 0) {
                ctx.moveTo(0, data[0]);
                for (let i = 1; i < data.length; i++) {
                    ctx.lineTo(i, data[i]);
                }
            }
            ctx.stroke();

            // Draw the moving point at the end
            const endPointX = data.length - 1;
            const endPointY = data[endPointX];
            ctx.fillStyle = '#f00';
            ctx.beginPath();
            ctx.arc(endPointX, endPointY, 5, 0, Math.PI * 2); // End point as a circle
            ctx.fill();

            // Update data for next frame
            if (x < width) {
                const newY = generateECG(x, isFlat);
                data.push(newY);
                x++;
            } else {
                // Reset when the graph reaches the canvas boundary
                data = [];
                x = 0;
            }

            // Update elapsed time
            timeElapsed += 1000 / 60; // Increment by frame time (60 FPS)
        }

        // Update the chart at 60 FPS
        setInterval(draw, 1000 / 60);
    }
}