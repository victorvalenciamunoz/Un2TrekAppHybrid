export function addBounceAndChangeColor(markers, markerId, newIconUrl) {
    console.log("Llegamos");
    //console.log(map);

    var marker = markers[markerId];


    // A�adir la animaci�n de rebote
    marker.setAnimation(google.maps.Animation.BOUNCE);

    // Detener la animaci�n despu�s de 700 ms
    setTimeout(function () {
        marker.setAnimation(null);

        // Cambiar el icono del marcador
        marker.setIcon(newIconUrl);
    }, 700);
}