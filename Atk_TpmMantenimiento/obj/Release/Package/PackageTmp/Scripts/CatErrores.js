function openmodal(url) {
    // Hace una petición get y carga el formulario en la ventana modal
    $('#contentModal').load(url, function () {
        $('#modalGenerica').modal({
            keyboard: true
        }, 'show');
        // Suscribe el evento submit
        bindForm(this);
    });
}