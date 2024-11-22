export function addBounceAndChangeColor(markers, markerId, newIconUrl) {
    console.log("Llegamos");
    //console.log(map);

    var marker = markers[markerId];


    // Añadir la animación de rebote
    marker.setAnimation(google.maps.Animation.BOUNCE);

    // Detener la animación después de 700 ms
    setTimeout(function () {
        marker.setAnimation(null);

        // Cambiar el icono del marcador
        marker.setIcon(newIconUrl);
    }, 700);
}