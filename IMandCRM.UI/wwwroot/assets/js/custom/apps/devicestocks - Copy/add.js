// Class definition
var KTModalStockPointsAdd = function () {
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
                    'StockName': {
                        validators: {
                            notEmpty: {
                                message: 'Stok noktası alanının doldurulması zorunludur.'
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
                                $('#kt_modal_add_stockpoint_form').submit();
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
            modal = new bootstrap.Modal(document.querySelector('#kt_modal_add_stockpoint'));
            form = document.querySelector('#kt_modal_add_stockpoint_form');

            submitButton = form.querySelector('#kt_modal_add_stockpoint_submit');
            cancelButton = form.querySelector('#kt_modal_add_stockpoint_cancel');
            closeButton = form.querySelector('#kt_modal_add_stockpoint_close');

            handleForm();
        }
    };
}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTModalStockPointsAdd.init();
});