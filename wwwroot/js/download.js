// Função para download de arquivo CSV
window.downloadFile = function (filename, base64Content) {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:text/csv;charset=utf-8;base64," + base64Content;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
