// Class definition
var KTModalTechSupportsAdd = function () {
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
                    'FirmIdKod': {
                        validators: {
                            notEmpty: {
                                message: 'Firma alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'SupportType': {
                        validators: {
                            notEmpty: {
                                message: 'Destek tipi alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'Description': {
                        validators: {
                            notEmpty: {
                                message: 'Açıklama alanının doldurulması zorunludur.'
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
                                $('#kt_techsupports_add_form').submit();
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
            form = document.querySelector('#kt_techsupports_add_form');
            submitButton = form.querySelector('#kt_add_techsupport_submit');

            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalTechSupportsAdd.init();
});