tinymce.init({
    selector: '#TaskDescription',
    height: 400,
    plugins: [
        'advlist', 'autolink', 'lists', 'link', 'image', 'charmap', 'preview',
        'anchor', 'searchreplace', 'visualblocks', 'code', 'fullscreen',
        'insertdatetime', 'media', 'table', 'help', 'wordcount', 'emoticons',
        'codesample', 'paste', 'hr'
    ],
    toolbar1: 'undo redo | styles | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify',
    toolbar2: 'bullist numlist | outdent indent | link image media | forecolor backcolor emoticons | code fullscreen',
    menubar: 'file edit view insert format tools table help',
    toolbar_mode: 'sliding',
    contextmenu: 'link image table',
    content_css: '//www.tiny.cloud/css/codepen.min.css',
    content_style: 'body { font-family: Helvetica, Arial, sans-serif; font-size: 14px; }',
    paste_data_images: true,
    branding: false,
    promotion: false,
    setup: function (editor) {
        editor.on('change', function () {
            editor.save();
        });
    },
    image_title: true,
    automatic_uploads: true,
    file_picker_types: 'image',
    style_formats: [
        {
            title: 'Headings',
            items: [
                { title: 'Heading 1', format: 'h1' },
                { title: 'Heading 2', format: 'h2' },
                { title: 'Heading 3', format: 'h3' }
            ]
        },
        {
            title: 'Inline',
            items: [
                { title: 'Bold', format: 'bold' },
                { title: 'Italic', format: 'italic' },
                { title: 'Underline', format: 'underline' },
                { title: 'Strikethrough', format: 'strikethrough' },
                { title: 'Code', format: 'code' }
            ]
        }
    ]
});