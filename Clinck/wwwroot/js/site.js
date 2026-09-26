var $lastClickedBtn = null;

function showErrorMessage(xhr) {
    toastr.error("Failed to process your request. Please try again.");
}

// ============================================================
// Initialize Bootstrap Dropdowns (Metronic standard)
// ============================================================
function reinitDropdowns() {
    var dropdownElementList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="dropdown"]')
    );

    dropdownElementList.map(function (dropdownToggleEl) {
        return bootstrap.Dropdown.getInstance(dropdownToggleEl)
            || new bootstrap.Dropdown(dropdownToggleEl);
    });
}

// ============================================================
// Open Modal - Add / Edit
// ============================================================
$(document).on(
    'click',
    '#model-add-edit-js, [data-bs-target="#Modal"]',
    function (e) {
        e.preventDefault();

        $lastClickedBtn = $(this);

        var url =
            $lastClickedBtn.data('url')
            || $lastClickedBtn.attr('href');

        var title =
            $lastClickedBtn.data('title')
            || 'Management';

        $('#Modal .modal-title').text(title);

        $('#Modal .modal-body').html(
            '<div class="text-center py-4">' +
            '<div class="spinner-border text-primary" role="status"></div>' +
            '</div>'
        );

        if (!url || url === 'javascript:;') {
            return;
        }

        $.get(url, function (data) {
            $('#Modal .modal-body').html(data);

            var form = $('#ModalForm');

            form.removeData('validator');
            form.removeData('unobtrusiveValidation');

            if ($.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse(form);
            }

            var modalElement = document.getElementById('Modal');
            var myModal =
                bootstrap.Modal.getInstance(modalElement)
                || new bootstrap.Modal(modalElement);

            myModal.show();

        }).fail(function (xhr) {
            $('#Modal .modal-body').html(
                '<div class="alert alert-danger text-center">' +
                'Failed to load content. Status: ' +
                xhr.status +
                '</div>'
            );

            var modalElement = document.getElementById('Modal');
            var myModal =
                bootstrap.Modal.getInstance(modalElement)
                || new bootstrap.Modal(modalElement);

            myModal.show();
        });
    }
);

// ============================================================
// Modal Form Submit (Create & Edit Live Update)
// ============================================================
$(document).on('submit', '#ModalForm', function (e) {
    e.preventDefault();

    var form = $(this);

    // Validation Check
    if ($.validator && !form.valid()) {
        return;
    }

    var submitBtn = $('#Modal').find('button[type="submit"]');

    submitBtn.find('.indicator-label').hide();
    submitBtn.find('.indicator-progress').show();
    submitBtn.prop('disabled', true);

    var url = form.attr('action');
    var formData = form.serialize();

    var token = $('input[name="__RequestVerificationToken"]').val();
    if (token) {
        formData += '&__RequestVerificationToken=' + encodeURIComponent(token);
    }

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        success: function (response) {
            var modalElement = document.getElementById('Modal');
            var modal = bootstrap.Modal.getInstance(modalElement);

            if (
                typeof response === "string" &&
                response.trim().startsWith("<tr")
            ) {
                if (modal) {
                    modal.hide();
                }

                toastr.success(
                    "Operation completed successfully!",
                    "Success",
                    {
                        closeButton: true,
                        progressBar: true,
                        positionClass: "toast-top-right"
                    }
                );

                var table = $('#kt_table').DataTable();
                var $newRow = $(response);

                var itemId = null;
                if ($lastClickedBtn) {
                    itemId = $lastClickedBtn.data('id');
                }
                if (!itemId) {
                    itemId = form.find('input[name="Id"]').val();
                }

                // حذف السطر القديم إن وجد (للتعديل)
                if (itemId && itemId > 0) {
                    table.rows(function (idx, data, node) {
                        return $(node).find('[data-id="' + itemId + '"]').length > 0 ||
                            $(node).find('a[data-url*="id=' + itemId + '"]').length > 0;
                    }).remove();
                }

                // إضافة السطر بطريقة سليمة للـ DataTable
                table.row.add($newRow[0]).draw(false);

                // إعادة تفعيل القوائم المنسدلة
                reinitDropdowns();

                $lastClickedBtn = null;
            }
            else {
                $('#Modal .modal-body').html(response);

                var reParsedForm = $('#ModalForm');
                reParsedForm.removeData('validator');
                reParsedForm.removeData('unobtrusiveValidation');

                if ($.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse(reParsedForm);
                }
            }
        },
        error: function (xhr) {
            console.log("Status:", xhr.status);
            console.log("Response Text:", xhr.responseText);
            showErrorMessage(xhr);
        },
        complete: function () {
            submitBtn.find('.indicator-label').show();
            submitBtn.find('.indicator-progress').hide();
            submitBtn.prop('disabled', false);
        }
    });
});

// ============================================================
// Toggle Department Status (Bootbox & Live DOM Update)
// ============================================================
$(document).on('click', '.js-toggle-status', function (e) {
    e.preventDefault();

    var btn = $(this);
    var url = btn.data('url');
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (!url) {
        return;
    }

    bootbox.confirm({
        message: 'Are you sure you want to toggle status?',
        buttons: {
            confirm: { label: 'Yes', className: 'btn-success' },
            cancel: { label: 'No', className: 'btn-danger' }
        },
        callback: function (result) {
            if (!result) return;

            $.ajax({
                url: url,
                type: 'POST',
                data: {
                    '__RequestVerificationToken': token
                },
                success: function (lastUpdatedOn) {
                    var row = btn.closest('tr');
                    var badge = row.find('.js-status-badge');

                    var isCurrentlyActive = badge.hasClass('badge-light-success');

                    if (isCurrentlyActive) {
                        badge
                            .removeClass('badge-light-success text-success')
                            .addClass('badge-light-danger text-danger')
                            .html('<i class="fa-solid fa-circle-xmark me-1 text-danger"></i> Inactive');
                    }
                    else {
                        badge
                            .removeClass('badge-light-danger text-danger')
                            .addClass('badge-light-success text-success')
                            .html('<i class="fa-solid fa-circle-check me-1 text-success"></i> Active');
                    }

                    row.find('.js-updated-on').html(lastUpdatedOn);

                    if ($.fn.DataTable.isDataTable('#kt_table')) {
                        var table = $('#kt_table').DataTable();

                        table.row(row).invalidate().draw(false);
                    }

                    row.addClass('animate__animated animate__flash');
                    row.one('animationend webkitAnimationEnd oAnimationEnd', function () {
                        row.removeClass('animate__animated animate__flash');
                    });

                    toastr.success("Status updated successfully!", "Success");
                },
                error: function (xhr) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!'
                    });
                }
            });
        }
    });
});

// ============================================================
// Document Ready
// ============================================================
$(document).ready(function () {

    var tableElement = $('#kt_table');

    if (tableElement.length > 0) {

        // Initialize DataTable only once
        if (!$.fn.DataTable.isDataTable('#kt_table')) {

            var table = tableElement.DataTable({

                pageLength: 10,

                lengthMenu: [
                    [5, 10, 25, 50],
                    [5, 10, 25, 50]
                ],

                columnDefs: [
                    {
                        orderable: false,
                        targets: 'no-sort'
                    }
                ],

                language: {
                    search: '',
                    searchPlaceholder: 'Search...',
                    lengthMenu: 'Show _MENU_',
                    info: 'Showing _START_ to _END_ of _TOTAL_ records',

                    paginate: {
                        first: 'First',
                        last: 'Last',
                        next: '<i class="fa-solid fa-chevron-right fs-7"></i>',
                        previous: '<i class="fa-solid fa-chevron-left fs-7"></i>'
                    }
                },

                dom:
                    "<'row mb-5'" +
                    "<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'l>" +
                    "<'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'f>" +
                    ">" +

                    "<'table-responsive'tr>" +

                    "<'row'" +
                    "<'col-sm-12 col-md-5 d-flex align-items-center justify-content-center justify-content-md-start'i>" +
                    "<'col-sm-12 col-md-7 d-flex align-items-center justify-content-center justify-content-md-end'p>" +
                    ">",

                drawCallback: function () {
                    reinitDropdowns();
                }
            });

            // Search input styling
            $('#kt_table_filter input')
                .addClass('form-control form-control-solid w-250px')
                .css('margin-left', '10px');

            // Length select styling
            $('#kt_table_length select')
                .addClass('form-select form-select-solid')
                .css('width', 'auto');

        } else {

            // Get existing DataTable instance
            var table = tableElement.DataTable();

            table.draw(false);
        }
    }

    reinitDropdowns();
});