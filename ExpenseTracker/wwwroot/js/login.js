document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('profileImage').addEventListener('change', function () {
        var fileName = this.files[0].name;
        var displayedFileName = fileName.length > 8 ? fileName.substring(0, 8) + '...' : fileName;
        document.getElementById('selectedFileName').innerText = displayedFileName;
    });

    //Logic of inputs 
    document.getElementById('chk').addEventListener('change', function () {
        var isChecked = this.checked;
        var inputFields = document.querySelectorAll('input[type="text"], input[type="password"], input[type="file"]');
        inputFields.forEach(function (input) {
            input.value = '';
        });
        document.getElementById('selectedFileName').innerText = '';

        // If the checkbox is checked, reset the register form
        if (isChecked) {
            var registerInputFields = document.querySelectorAll('.signup input[type="text"], .signup input[type="password"], .signup input[type="file"]');
            registerInputFields.forEach(function (input) {
                input.value = '';
            });
            document.getElementById('selectedFileName').innerText = '';
        }
    });

   
});
