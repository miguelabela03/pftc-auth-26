(function () {
    const fileInput = document.getElementById('PostImage');
    const selectBtn = document.getElementById('SelectImageBtn');
    const replaceBtn = document.getElementById('ReplaceImageBtn');
    const removeBtn = document.getElementById('RemoveImageBtn');
    const previewContainer = document.getElementById('ImagePreviewContainer');
    const preview = document.getElementById('ImagePreview');
    const fileName = document.getElementById('ImageFileName');

    function showPreview(file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            fileName.textContent = file.name;
            previewContainer.classList.remove('d-none');
            selectBtn.classList.add('d-none');
        };
        reader.readAsDataURL(file);
    }

    function clearImage() {
        fileInput.value = '';
        preview.src = '#';
        fileName.textContent = '';
        previewContainer.classList.add('d-none');
        selectBtn.classList.remove('d-none');
    }

    selectBtn.addEventListener('click', function () {
        fileInput.click();
    });

    replaceBtn.addEventListener('click', function () {
        fileInput.click();
    });

    removeBtn.addEventListener('click', function () {
        clearImage();
    });

    fileInput.addEventListener('change', function () {
        if (fileInput.files && fileInput.files[0]) {
            showPreview(fileInput.files[0]);
        }
    });
})();

