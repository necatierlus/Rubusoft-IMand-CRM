
function EditAddress(addressIdKod) {
    debugger;
    $.ajax({
        url: '/Address/GetFirmAddressByIdKod?addressIdKod=' + addressIdKod,
        type: 'GET',
        success: function (response) {
            console.log(response);
            if (response.result) {
                $('#kt_modal_edit_address_form [name="IdKod"]').val(response.data.idKod);
                $('#kt_modal_edit_address_form [name="FirmIdKod"]').val(response.data.firmIdKod);
                $('#kt_modal_edit_address_form [name="AddressTitle"]').val(response.data.addressTitle);
                $('#kt_modal_edit_address_form [name="CountryId"]').val(response.data.countryId).change();
                $('#kt_modal_edit_address_form [name="CityId"]').val(response.data.cityId).change();
                $('#kt_modal_edit_address_form [name="DistrictId"]').val(response.data.districtId).change();
                $('#kt_modal_edit_address_form [name="AddressDescription"]').val(response.data.addressDescription);
            } else {
                toastr.error("Adres bulunamadı.");
            }
        },
        error: function (error) {
            toastr.error("Kayıt yüklenirken hata oluştu.");
        }
    });


}
// Class definition
var KTModalAddressesEdit = function () {
    var editSubmitButton;
    var editCancelButton;
    var editCloseButton;
    var editValidator;
    var editForm;
    var editModal;

    // Init form inputs
    var handleForm = function () {
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        editValidator = FormValidation.formValidation(
            editForm,
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
        editSubmitButton.addEventListener('click', function (e) {
            e.preventDefault();
            if (editValidator) {
                editValidator.validate().then(function (status) {
                    console.log('validated!');

                    if (status == 'Valid') {
                        Swal.fire({
                            title: "Güncellemek istediğinizden emin misiniz?",
                            icon: "warning",
                            showCancelButton: true,
                            cancelButtonText: "Hayır!",
                            confirmButtonText: "Evet!"
                        }).then(function (result) {
                            if (result.value) {
                                $('#kt_modal_edit_address_form').submit();
                            }
                        });
                    }
                });
            }
        });

        editCancelButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "İptal istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    editValidator.resetForm();
                    editModal.hide();
                    editForm.reset();
                    $('#kt_modal_edit_address_form [name="CountryId"]').val(208).change();
                    $('#kt_modal_edit_address_form [name="CityId"]').val("").change();
                    $('#kt_modal_edit_address_form [name="DistrictId"]').val("").change();
                }
            });

        });

        editCloseButton.addEventListener('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: "Kapatmak istediğinizden emin misiniz?",
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "Hayır!",
                confirmButtonText: "Evet!"
            }).then(function (result) {
                if (result.value) {
                    editValidator.resetForm();
                    editModal.hide();
                    editForm.reset();
                    $('#kt_modal_edit_address_form [name="CountryId"]').val(208).change();
                    $('#kt_modal_edit_address_form [name="CityId"]').val("").change();
                    $('#kt_modal_edit_address_form [name="DistrictId"]').val("").change();
                }
            });

        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            editModal = new bootstrap.Modal(document.querySelector('#kt_modal_edit_address'));
            editForm = document.querySelector('#kt_modal_edit_address_form');
            editSubmitButton = editForm.querySelector('#kt_modal_edit_address_submit');
            editCancelButton = editForm.querySelector('#kt_modal_edit_address_cancel');
            editCloseButton = document.querySelector('#kt_modal_edit_address_close');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalAddressesEdit.init();
});