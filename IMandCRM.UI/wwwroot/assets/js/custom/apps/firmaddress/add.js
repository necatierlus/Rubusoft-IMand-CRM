// Class definition
var KTModalAddressesAdd = function () {
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
                    'AddressTitle': {
                        validators: {
                            notEmpty: {
                                message: 'Adres başlık alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'CountryId': {
                        validators: {
                            notEmpty: {
                                message: 'Ülke alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'CityId': {
                        validators: {
                            notEmpty: {
                                message: 'İl alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'DistrictId': {
                        validators: {
                            notEmpty: {
                                message: 'İlçe alanının doldurulması zorunludur.'
                            }
                        }
                    },
                    'AddressDescription': {
                        validators: {
                            notEmpty: {
                                message: 'Detay alanının doldurulması zorunludur.'
                            }
                        }
                    },

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
                                $('#kt_modal_new_address_form').submit();
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
                    $('#kt_modal_new_address_form [name="CountryId"]').val(208).change();
                    $('#kt_modal_new_address_form [name="CityId"]').val("").change();
                    $('#kt_modal_new_address_form [name="DistrictId"]').val("").change();
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
                    $('#kt_modal_new_address_form [name="CountryId"]').val(208).change();
                    $('#kt_modal_new_address_form [name="CityId"]').val("").change();
                    $('#kt_modal_new_address_form [name="DistrictId"]').val("").change();
                }
            });

        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            newModal = new bootstrap.Modal(document.querySelector('#kt_modal_new_address'));
            newForm = document.querySelector('#kt_modal_new_address_form');
            newSubmitButton = newForm.querySelector('#kt_modal_new_address_submit');
            newCancelButton = newForm.querySelector('#kt_modal_new_address_cancel');
            newCloseButton = document.querySelector('#kt_modal_new_address_close');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalAddressesAdd.init();
});