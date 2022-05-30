
function EditManager(managerIdKod) {
    debugger;
    $.ajax({
        url: '/FirmManager/GetFirmManagerByIdKod?managerIdKod=' + managerIdKod,
        type: 'GET',
        success: function (response) {
            console.log(response);
            if (response.result) {
                $('#kt_modal_edit_manager_form [name="IdKod"]').val(response.data.idKod);
                $('#kt_modal_edit_manager_form [name="FirmIdKod"]').val(response.data.firmIdKod);
                $('#kt_modal_edit_manager_form [name="FirstName"]').val(response.data.firstName);
                $('#kt_modal_edit_manager_form [name="LastName"]').val(response.data.lastName);
                $('#kt_modal_edit_manager_form [name="EMail"]').val(response.data.eMail);
                $('#kt_modal_edit_manager_form [name="Title"]').val(response.data.title);
            } else {
                toastr.error("Yönetici bulunamadı.");
            }
        },
        error: function (error) {
            toastr.error("Kayıt yüklenirken hata oluştu.");
        }
    });


}
// Class definition
var KTModalManagersEdit = function () {
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
                                $('#kt_modal_edit_manager_form').submit();
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
                }
            });

        });

    }

    return {
        // Public functions
        init: function () {
            // Elements
            editModal = new bootstrap.Modal(document.querySelector('#kt_modal_edit_manager'));
            editForm = document.querySelector('#kt_modal_edit_manager_form');
            editSubmitButton = editForm.querySelector('#kt_modal_edit_manager_submit');
            editCancelButton = editForm.querySelector('#kt_modal_edit_manager_cancel');
            editCloseButton = document.querySelector('#kt_modal_edit_manager_close');
            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalManagersEdit.init();
});