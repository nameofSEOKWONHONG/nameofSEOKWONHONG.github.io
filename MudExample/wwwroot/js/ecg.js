window.ecgInterop = {
    ecgLoad: function () {
        const canvas = document.getElementById('ecgCanvas');
        const ctx = canvas.getContext('2d');
        const width = canvas.width;
        const height = canvas.height;

        let data = []; // Stores ECG data
        let x = 0; // Current x-coordinate of the end point

        // Generate a pseudo-ECG waveform
        function generateECG(x) {
            return (
                height / 2 +
                Math.sin(x * 0.1) * 30 + // Main waveform
                Math.sin(x * 0.02) * 10 // Smaller oscillations
            );
        }

        function draw() {
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
                const newY = generateECG(x);
                data.push(newY);
                x++;
            } else {
                // Reset when the graph reaches the canvas boundary
                data = [];
                x = 0;
            }
        }

        // Update the chart at 60 FPS
        setInterval(draw, 1000 / 60);
    }
}