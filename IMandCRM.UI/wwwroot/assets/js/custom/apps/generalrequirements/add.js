
// Class definition
var KTModalGeneralRequirementsAdd = function () {
    var submitButton;
    var cancelButton;
    var closeButton;
    var validator;
    var form;
    var modal;

    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'RequirementTitle': {
                        validators: {
                            notEmpty: {
                                message: 'Başlık alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'RequirementContent': {
                        validators: {
                            notEmpty: {
                                message: 'İçerik alanının doldurulması zorunludur.'
                            }
                        }
                    }

                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleInvalidClass: '',
                        eleValidClass: ''
                    })
                }
            }
        );

        // Action buttons
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            if (validator) {
                validator.validate().then(function (status) {
                    console.log('validated!');

                    if (status == 'Valid') {
                        Swal.fire({
                            title: "Kaydetmek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                $('#kt_modal_add_generalrequirement_form').submit();
                            }
                        });

                    } else {

                    }
                });
            }
        });

        cancelButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "İptal istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    validator.resetForm();
                    modal.hide();
                    form.reset();
                }
            });

        });

        closeButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "Kapatmak istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    validator.resetForm();
                    modal.hide();
                    form.reset();
                }
            });

        });

        
    }

    return {
        // Public functions
        init: function () {
            // Elements
            modal = new bootstrap.Modal(document.querySelector('#kt_modal_add_generalrequirement'));
            form = document.querySelector('#kt_modal_add_generalrequirement_form');

            // multi select
            $('[name="Devices"]').select2({
                placeholder: "Cihaz Seçiniz...",
            });

            $.ajax({
                url: '/Device/GetDevices',
                type: 'GET',
                success: function (response) {
                    $.each(response, function (ind, device) {
                        $('[name="Devices"]').append(`<option value="${device.idKod}">${device.deviceName}</option>`);
                    })
                },
                error: function (error) {

                }
            });

            submitButton = form.querySelector('#kt_modal_add_generalrequirement_submit');
            cancelButton = form.querySelector('#kt_modal_add_generalrequirement_cancel');
            closeButton = form.querySelector('#kt_modal_add_generalrequirement_close');

            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalGeneralRequirementsAdd.init();
});
