function initImageUploader(id) {
    const div = document.getElementById(`div_${id}`);
    const input = document.getElementById(`input_${id}`);
    const img = document.getElementById(`img_${id}`);
    input.addEventListener('change', function (e) {
        if (input.files && input.files[0]) {
            const reader = new FileReader();
            reader.onload = function (e) {
                img.src = e.target.result;
            };
            reader.readAsDataURL(input.files[0]);
        }
    });
    div.addEventListener('click', () => input.click());
}