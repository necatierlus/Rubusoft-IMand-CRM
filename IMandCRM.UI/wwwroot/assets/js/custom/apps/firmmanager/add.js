// Class definition
var KTModalManagersAdd = function () {
    var newSubmitButton;
    var newCancelButton;
    var newCloseButton;
    var newValidator;
    var newForm;
    var newModal;


    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        newValidator = FormValidation.formValidation(
            newForm,
            {
                fields: {
                    'FirstName': {
                        validators: {
                            notEmpty: {
                                message: 'Ad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'LastName': {
                        validators: {
                            notEmpty: {
                                message: 'Soyad alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'EMail': {
                        validators: {
                            notEmpty: {
                                message: 'E-Mail alanının doldurulması zorunludur.'
                            },
                            emailAddress: {
                                message: "Lütfen geçerli bir E-Mail adresi giriniz."
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
        newSubmitButton.addEventListener('click', function (e) {
            e.preventDefault();
            if (newValidator) {
                newValidator.validate().then(function (status) {
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
                                $('#kt_modal_new_manager_form').submit();
                            }
                        });
                    }
                });
            }
        });

        newCancelButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "İptal istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    newValidator.resetForm();
                    newModal.hide();
                    newForm.reset();
                }
            });

        });

        newCloseButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "Kapatmak istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    newValidator.resetForm();
                    newModal.hide();
                    newForm.reset();
                }
            });

        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            newModal = new bootstrap.Modal(document.querySelector('#kt_modal_new_manager'));
            newForm = document.querySelector('#kt_modal_new_manager_form');
            newSubmitButton = newForm.querySelector('#kt_modal_new_manager_submit');
            newCancelButton = newForm.querySelector('#kt_modal_new_manager_cancel');
            newCloseButton = document.querySelector('#kt_modal_new_manager_close');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalManagersAdd.init();
});