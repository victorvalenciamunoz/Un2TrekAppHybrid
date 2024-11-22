export function modifyDialogStyle() {
    const dialog = document.querySelector('fluent-dialog');
    if (dialog) {
        const controlElement = dialog.shadowRoot.querySelector('[part="control"]');
        if (controlElement) {
            controlElement.style.margin = '35px';
            controlElement.style.boxShadow = '5px 5px 50px #00E24E';
            controlElement.style.border = '2px solid #36C883';
            controlElement.style.backgroundColor = "rgba(6,52,7,.9)";
        }
    }
}