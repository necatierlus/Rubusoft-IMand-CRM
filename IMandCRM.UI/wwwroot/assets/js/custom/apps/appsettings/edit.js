// Class definition
var KTAppsettingsEdit = function () {
    var submitButton;
    var validator;
    var form;


    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'FirmName': {
                        validators: {
                            notEmpty: {
                                message: 'Firma adı alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'TradeName': {
                        validators: {
                            notEmpty: {
                                message: 'Ticari unvan alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'Email': {
                        validators: {
                            notEmpty: {
                                message: 'Mail alanının doldurulması zorunludur.'
                            },
                            emailAddress: {
                                message: "Lütfen geçerli bir E-Mail adresi giriniz."
                            }
                        }
                    },
                    'BidCode': {
                        validators: {
                            notEmpty: {
                                message: 'Teklif kodu alanının doldurulması zorunludur.'
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
            debugger;
            if (validator) {
                validator.validate().then(function (status) {
                    if (status == 'Valid') {
                        Swal.fire({
                            title: "Kaydetmek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                form.submit();
                            }
                        });

                    } else {

                    }
                });
            }
        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            form = document.querySelector('#kt_appsettings_edit_form');
            submitButton = form.querySelector('#kt_edit_appsetting_submit');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTAppsettingsEdit.init();
});