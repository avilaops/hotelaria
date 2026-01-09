// Menu Editor Application
class MenuEditor {
    constructor() {
        this.currentVersion = null;
        this.versions = [];
        this.history = [];
        this.historyIndex = -1;
        this.zoom = 1;
        this.editingCategory = null;
        this.editingItem = null;
        
        // Default menu data based on the image
        this.defaultMenuData = {
            name: "Menu Samba",
            theme: {
                bgColor: "#4a7171",
                textColor: "#ffffff",
                accentColor: "#ffd700",
                fontSize: 16,
                fontFamily: "'Segoe UI', sans-serif"
            },
            logo: "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 200 200'%3E%3Ccircle cx='100' cy='100' r='90' fill='%23ffd700' stroke='%23333' stroke-width='8'/%3E%3Ctext x='100' y='120' font-size='60' text-anchor='middle' fill='%23333' font-weight='bold'%3ESAMBA%3C/text%3E%3C/svg%3E",
            categories: [
                {
                    id: "soft-drinks",
                    name: "SOFT DRINKS",
                    icon: "🥤",
                    position: "left",
                    items: [
                        { name: "Coca Cola", price: "1,50€" },
                        { name: "Coca Cola Zero", price: "1,50€" },
                        { name: "Fanta Orange", price: "1,50€" },
                        { name: "Guaraná Antarctica", price: "1,50€" },
                        { name: "Lipton Ice Tea Lemon", price: "1,50€" },
                        { name: "Lipton Ice Tea Peach", price: "1,50€" }
                    ]
                },
                {
                    id: "juices",
                    name: "JUICES",
                    icon: "🧃",
                    position: "left",
                    items: [
                        { name: "Compal Peach", price: "1,50€" },
                        { name: "Compal Orange", price: "1,50€" }
                    ]
                },
                {
                    id: "energy",
                    name: "ENERGY DRINK",
                    icon: "⚡",
                    position: "left",
                    items: [
                        { name: "Red Bull", price: "3,00€" }
                    ]
                },
                {
                    id: "foods",
                    name: "FOODS",
                    icon: "🍕",
                    position: "left",
                    items: [
                        { name: "Pizza", price: "4,50€" },
                        { name: "Lasagna", price: "3,00€" }
                    ]
                },
                {
                    id: "water",
                    name: "WATER",
                    icon: "💧",
                    position: "right",
                    items: [
                        { name: "Still Water", price: "1,00€" },
                        { name: "Sparkling Water", price: "1,00€" }
                    ]
                },
                {
                    id: "beer",
                    name: "BEER & CIDER",
                    icon: "🍺",
                    position: "right",
                    items: [
                        { name: "Super Bock", price: "1,50€" },
                        { name: "Heineken", price: "2,00€" },
                        { name: "Somersby", price: "2,00€" }
                    ]
                },
                {
                    id: "wine",
                    name: "WINE",
                    icon: "🍷",
                    position: "right",
                    items: [
                        { name: "Red Wine", price: "5,00€" },
                        { name: "White Red", price: "5,00€" }
                    ]
                },
                {
                    id: "coffee",
                    name: "COFFEE",
                    icon: "☕",
                    position: "right",
                    items: [
                        { name: "Capsule Coffee", price: "1,00€" }
                    ]
                }
            ],
            footer: {
                text: "❄️Cold drinks available\n💳Cash & Card accepted"
            },
            extraServices: [
                { name: "Towel (Rent)", price: "3,00€" },
                { name: "Padlock", price: "3,00€" },
                { name: "Laundry Soap", price: "1,00€" },
                { name: "Washing Machine", price: "Free Use" }
            ]
        };
        
        this.init();
    }
    
    init() {
        this.loadVersions();
        this.setupEventListeners();
        
        // Create initial version if none exists
        if (this.versions.length === 0) {
            this.createNewVersion("Menu Original");
        } else {
            this.loadVersion(this.versions[0].id);
        }
    }
    
    setupEventListeners() {
        // Version management
        document.getElementById('btnNewVersion').addEventListener('click', () => {
            const name = prompt('Nome da nova versão:');
            if (name) this.createNewVersion(name);
        });
        
        // Theme controls
        document.getElementById('bgColor').addEventListener('input', (e) => this.updateTheme('bgColor', e.target.value));
        document.getElementById('textColor').addEventListener('input', (e) => this.updateTheme('textColor', e.target.value));
        document.getElementById('accentColor').addEventListener('input', (e) => this.updateTheme('accentColor', e.target.value));
        document.getElementById('fontSize').addEventListener('input', (e) => {
            this.updateTheme('fontSize', parseInt(e.target.value));
            document.getElementById('fontSizeValue').textContent = e.target.value + 'px';
        });
        document.getElementById('fontFamily').addEventListener('change', (e) => this.updateTheme('fontFamily', e.target.value));
        
        // Toolbar actions
        document.getElementById('btnSave').addEventListener('click', () => this.saveVersion());
        document.getElementById('btnUndo').addEventListener('click', () => this.undo());
        document.getElementById('btnRedo').addEventListener('click', () => this.redo());
        document.getElementById('btnZoomIn').addEventListener('click', () => this.adjustZoom(0.1));
        document.getElementById('btnZoomOut').addEventListener('click', () => this.adjustZoom(-0.1));
        document.getElementById('btnResetZoom').addEventListener('click', () => this.setZoom(1));
        
        // Export functions
        document.getElementById('btnExportPNG').addEventListener('click', () => this.exportAsPNG());
        document.getElementById('btnExportPDF').addEventListener('click', () => this.exportAsPDF());
        document.getElementById('btnPrint').addEventListener('click', () => window.print());
        
        // Add items
        document.getElementById('btnAddCategory').addEventListener('click', () => this.showCategoryModal());
        document.getElementById('btnAddItem').addEventListener('click', () => this.showItemModal());
        
        // Category modal
        document.getElementById('closeCategoryModal').addEventListener('click', () => this.hideCategoryModal());
        document.getElementById('cancelCategory').addEventListener('click', () => this.hideCategoryModal());
        document.getElementById('categoryForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.saveCategoryModal();
        });
        
        // Item modal
        document.getElementById('closeItemModal').addEventListener('click', () => this.hideItemModal());
        document.getElementById('cancelItem').addEventListener('click', () => this.hideItemModal());
        document.getElementById('itemForm').addEventListener('submit', (e) => {
            e.preventDefault();
            this.saveItemModal();
        });
        
        // Close modals on outside click
        window.addEventListener('click', (e) => {
            if (e.target.classList.contains('modal')) {
                e.target.classList.remove('show');
            }
        });
    }
    
    // Version Management
    loadVersions() {
        const saved = localStorage.getItem('menuVersions');
        this.versions = saved ? JSON.parse(saved) : [];
    }
    
    saveVersions() {
        localStorage.setItem('menuVersions', JSON.stringify(this.versions));
        this.renderVersionsList();
    }
    
    createNewVersion(name) {
        const version = {
            id: Date.now().toString(),
            name: name,
            createdAt: new Date().toISOString(),
            data: JSON.parse(JSON.stringify(this.defaultMenuData))
        };
        
        this.versions.unshift(version);
        this.saveVersions();
        this.loadVersion(version.id);
    }
    
    loadVersion(versionId) {
        const version = this.versions.find(v => v.id === versionId);
        if (!version) return;
        
        this.currentVersion = version;
        this.renderMenu(version.data);
        this.applyTheme(version.data.theme);
        this.renderVersionsList();
        this.saveToHistory();
    }
    
    deleteVersion(versionId) {
        if (!confirm('Tem certeza que deseja excluir esta versão?')) return;
        
        this.versions = this.versions.filter(v => v.id !== versionId);
        this.saveVersions();
        
        if (this.versions.length === 0) {
            this.createNewVersion("Menu Original");
        } else if (this.currentVersion && this.currentVersion.id === versionId) {
            this.loadVersion(this.versions[0].id);
        }
    }
    
    duplicateVersion(versionId) {
        const version = this.versions.find(v => v.id === versionId);
        if (!version) return;
        
        const newVersion = {
            id: Date.now().toString(),
            name: version.name + " (Cópia)",
            createdAt: new Date().toISOString(),
            data: JSON.parse(JSON.stringify(version.data))
        };
        
        this.versions.unshift(newVersion);
        this.saveVersions();
        this.loadVersion(newVersion.id);
    }
    
    saveVersion() {
        if (!this.currentVersion) return;
        
        this.currentVersion.data = this.getCurrentMenuData();
        this.saveVersions();
        
        // Show save confirmation
        const btn = document.getElementById('btnSave');
        const originalText = btn.innerHTML;
        btn.innerHTML = '<i class="fas fa-check"></i> Salvo!';
        btn.style.background = '#2ecc71';
        btn.style.color = 'white';
        
        setTimeout(() => {
            btn.innerHTML = originalText;
            btn.style.background = '';
            btn.style.color = '';
        }, 2000);
    }
    
    renderVersionsList() {
        const container = document.getElementById('versionsList');
        const newBtn = container.querySelector('.btn-new-version');
        
        container.innerHTML = '';
        container.appendChild(newBtn);
        
        this.versions.forEach(version => {
            const item = document.createElement('div');
            item.className = 'version-item';
            if (this.currentVersion && this.currentVersion.id === version.id) {
                item.classList.add('active');
            }
            
            item.innerHTML = `
                <div>
                    <strong>${version.name}</strong><br>
                    <small>${new Date(version.createdAt).toLocaleString('pt-PT')}</small>
                </div>
                <div class="version-actions">
                    <button class="btn-version-action" onclick="menuEditor.duplicateVersion('${version.id}')" title="Duplicar">
                        <i class="fas fa-copy"></i>
                    </button>
                    <button class="btn-version-action" onclick="menuEditor.deleteVersion('${version.id}')" title="Excluir">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            `;
            
            item.addEventListener('click', (e) => {
                if (!e.target.closest('.version-actions')) {
                    this.loadVersion(version.id);
                }
            });
            
            container.appendChild(item);
        });
    }
    
    // Menu Rendering
    renderMenu(menuData) {
        const content = document.getElementById('menuContent');
        content.innerHTML = '';
        
        // Create two columns
        const leftColumn = document.createElement('div');
        const rightColumn = document.createElement('div');
        leftColumn.style.display = 'flex';
        leftColumn.style.flexDirection = 'column';
        leftColumn.style.gap = '20px';
        rightColumn.style.display = 'flex';
        rightColumn.style.flexDirection = 'column';
        rightColumn.style.gap = '20px';
        
        menuData.categories.forEach(category => {
            const categoryEl = this.createCategoryElement(category);
            if (category.position === 'left') {
                leftColumn.appendChild(categoryEl);
            } else {
                rightColumn.appendChild(categoryEl);
            }
        });
        
        content.appendChild(leftColumn);
        content.appendChild(rightColumn);
        
        // Update footer
        document.querySelector('.footer-note').innerHTML = menuData.footer.text.split('\n').map(line => `<p>${line}</p>`).join('');
    }
    
    createCategoryElement(category) {
        const div = document.createElement('div');
        div.className = 'menu-category';
        div.dataset.categoryId = category.id;
        
        const itemsHTML = category.items.map((item, index) => `
            <div class="menu-item" data-item-index="${index}">
                <span class="item-name" contenteditable="true">${item.name}</span>
                <span class="item-price" contenteditable="true">${item.price}</span>
                <div class="item-actions">
                    <button class="btn-item-action" onclick="menuEditor.deleteItem('${category.id}', ${index})">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </div>
        `).join('');
        
        div.innerHTML = `
            <div class="category-header">
                <span class="category-icon">${category.icon}</span>
                <span class="category-title" contenteditable="true">${category.name}</span>
            </div>
            <div class="category-actions">
                <button class="btn-category-action" onclick="menuEditor.editCategory('${category.id}')" title="Editar">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn-category-action" onclick="menuEditor.deleteCategory('${category.id}')" title="Excluir">
                    <i class="fas fa-trash"></i>
                </button>
                <button class="btn-category-action" onclick="menuEditor.addItemToCategory('${category.id}')" title="Adicionar Item">
                    <i class="fas fa-plus"></i>
                </button>
            </div>
            <div class="menu-items">
                ${itemsHTML}
            </div>
        `;
        
        // Add contenteditable listeners
        div.querySelectorAll('[contenteditable="true"]').forEach(el => {
            el.addEventListener('blur', () => this.onContentEdit());
        });
        
        return div;
    }
    
    onContentEdit() {
        this.saveToHistory();
    }
    
    getCurrentMenuData() {
        const data = {
            name: this.currentVersion.data.name,
            theme: { ...this.currentVersion.data.theme },
            logo: this.currentVersion.data.logo,
            categories: [],
            footer: {
                text: document.querySelector('.footer-note').innerText
            }
        };
        
        document.querySelectorAll('.menu-category').forEach(catEl => {
            const categoryId = catEl.dataset.categoryId;
            const originalCat = this.currentVersion.data.categories.find(c => c.id === categoryId);
            
            const category = {
                id: categoryId,
                name: catEl.querySelector('.category-title').textContent.trim(),
                icon: catEl.querySelector('.category-icon').textContent.trim(),
                position: originalCat ? originalCat.position : 'left',
                items: []
            };
            
            catEl.querySelectorAll('.menu-item').forEach(itemEl => {
                category.items.push({
                    name: itemEl.querySelector('.item-name').textContent.trim(),
                    price: itemEl.querySelector('.item-price').textContent.trim()
                });
            });
            
            data.categories.push(category);
        });
        
        return data;
    }
    
    // Theme Management
    applyTheme(theme) {
        const canvas = document.getElementById('menuCanvas');
        canvas.style.background = `linear-gradient(135deg, ${theme.bgColor} 0%, ${this.adjustColor(theme.bgColor, 20)} 100%)`;
        canvas.style.color = theme.textColor;
        canvas.style.fontSize = theme.fontSize + 'px';
        canvas.style.fontFamily = theme.fontFamily;
        
        // Update theme controls
        document.getElementById('bgColor').value = theme.bgColor;
        document.getElementById('textColor').value = theme.textColor;
        document.getElementById('accentColor').value = theme.accentColor;
        document.getElementById('fontSize').value = theme.fontSize;
        document.getElementById('fontSizeValue').textContent = theme.fontSize + 'px';
        document.getElementById('fontFamily').value = theme.fontFamily;
        
        // Update accent colors
        document.querySelectorAll('.item-price').forEach(el => {
            el.style.color = theme.accentColor;
        });
    }
    
    updateTheme(property, value) {
        if (!this.currentVersion) return;
        
        this.currentVersion.data.theme[property] = value;
        this.applyTheme(this.currentVersion.data.theme);
        this.saveToHistory();
    }
    
    adjustColor(color, percent) {
        const num = parseInt(color.replace("#",""), 16);
        const amt = Math.round(2.55 * percent);
        const R = (num >> 16) + amt;
        const G = (num >> 8 & 0x00FF) + amt;
        const B = (num & 0x0000FF) + amt;
        return "#" + (0x1000000 + (R<255?R<1?0:R:255)*0x10000 +
            (G<255?G<1?0:G:255)*0x100 +
            (B<255?B<1?0:B:255))
            .toString(16).slice(1);
    }
    
    // Category Management
    showCategoryModal(categoryId = null) {
        const modal = document.getElementById('categoryModal');
        const form = document.getElementById('categoryForm');
        
        if (categoryId) {
            const category = this.currentVersion.data.categories.find(c => c.id === categoryId);
            if (!category) return;
            
            document.getElementById('categoryModalTitle').textContent = 'Editar Categoria';
            document.getElementById('categoryName').value = category.name;
            document.getElementById('categoryIcon').value = category.icon;
            document.getElementById('categoryPosition').value = category.position;
            this.editingCategory = categoryId;
        } else {
            document.getElementById('categoryModalTitle').textContent = 'Nova Categoria';
            form.reset();
            this.editingCategory = null;
        }
        
        modal.classList.add('show');
    }
    
    hideCategoryModal() {
        document.getElementById('categoryModal').classList.remove('show');
        this.editingCategory = null;
    }
    
    saveCategoryModal() {
        const name = document.getElementById('categoryName').value.trim();
        const icon = document.getElementById('categoryIcon').value.trim();
        const position = document.getElementById('categoryPosition').value;
        
        if (!name) return;
        
        if (this.editingCategory) {
            const category = this.currentVersion.data.categories.find(c => c.id === this.editingCategory);
            if (category) {
                category.name = name;
                category.icon = icon;
                category.position = position;
            }
        } else {
            const newCategory = {
                id: 'cat-' + Date.now(),
                name: name,
                icon: icon || '📁',
                position: position,
                items: []
            };
            this.currentVersion.data.categories.push(newCategory);
        }
        
        this.renderMenu(this.currentVersion.data);
        this.hideCategoryModal();
        this.saveToHistory();
    }
    
    editCategory(categoryId) {
        this.showCategoryModal(categoryId);
    }
    
    deleteCategory(categoryId) {
        if (!confirm('Tem certeza que deseja excluir esta categoria?')) return;
        
        this.currentVersion.data.categories = this.currentVersion.data.categories.filter(c => c.id !== categoryId);
        this.renderMenu(this.currentVersion.data);
        this.saveToHistory();
    }
    
    // Item Management
    showItemModal(categoryId = null, itemIndex = null) {
        const modal = document.getElementById('itemModal');
        const categorySelect = document.getElementById('itemCategory');
        
        // Populate category select
        categorySelect.innerHTML = this.currentVersion.data.categories.map(cat => 
            `<option value="${cat.id}">${cat.name}</option>`
        ).join('');
        
        if (categoryId) {
            categorySelect.value = categoryId;
            
            if (itemIndex !== null) {
                const category = this.currentVersion.data.categories.find(c => c.id === categoryId);
                const item = category.items[itemIndex];
                
                document.getElementById('itemModalTitle').textContent = 'Editar Item';
                document.getElementById('itemName').value = item.name;
                document.getElementById('itemPrice').value = item.price;
                this.editingItem = { categoryId, itemIndex };
            } else {
                document.getElementById('itemModalTitle').textContent = 'Novo Item';
                document.getElementById('itemForm').reset();
                categorySelect.value = categoryId;
                this.editingItem = null;
            }
        } else {
            document.getElementById('itemModalTitle').textContent = 'Novo Item';
            document.getElementById('itemForm').reset();
            this.editingItem = null;
        }
        
        modal.classList.add('show');
    }
    
    hideItemModal() {
        document.getElementById('itemModal').classList.remove('show');
        this.editingItem = null;
    }
    
    saveItemModal() {
        const categoryId = document.getElementById('itemCategory').value;
        const name = document.getElementById('itemName').value.trim();
        const price = document.getElementById('itemPrice').value.trim();
        
        if (!name || !price) return;
        
        const category = this.currentVersion.data.categories.find(c => c.id === categoryId);
        if (!category) return;
        
        if (this.editingItem) {
            category.items[this.editingItem.itemIndex] = { name, price };
        } else {
            category.items.push({ name, price });
        }
        
        this.renderMenu(this.currentVersion.data);
        this.hideItemModal();
        this.saveToHistory();
    }
    
    addItemToCategory(categoryId) {
        this.showItemModal(categoryId);
    }
    
    deleteItem(categoryId, itemIndex) {
        if (!confirm('Tem certeza que deseja excluir este item?')) return;
        
        const category = this.currentVersion.data.categories.find(c => c.id === categoryId);
        if (category) {
            category.items.splice(itemIndex, 1);
            this.renderMenu(this.currentVersion.data);
            this.saveToHistory();
        }
    }
    
    // History Management (Undo/Redo)
    saveToHistory() {
        if (!this.currentVersion) return;
        
        const state = JSON.stringify(this.getCurrentMenuData());
        
        // Remove any future history if we're not at the end
        this.history = this.history.slice(0, this.historyIndex + 1);
        
        // Add new state
        this.history.push(state);
        this.historyIndex = this.history.length - 1;
        
        // Limit history size
        if (this.history.length > 50) {
            this.history.shift();
            this.historyIndex--;
        }
    }
    
    undo() {
        if (this.historyIndex > 0) {
            this.historyIndex--;
            const state = JSON.parse(this.history[this.historyIndex]);
            this.currentVersion.data = state;
            this.renderMenu(state);
            this.applyTheme(state.theme);
        }
    }
    
    redo() {
        if (this.historyIndex < this.history.length - 1) {
            this.historyIndex++;
            const state = JSON.parse(this.history[this.historyIndex]);
            this.currentVersion.data = state;
            this.renderMenu(state);
            this.applyTheme(state.theme);
        }
    }
    
    // Zoom Management
    adjustZoom(delta) {
        this.setZoom(this.zoom + delta);
    }
    
    setZoom(value) {
        this.zoom = Math.max(0.5, Math.min(2, value));
        document.getElementById('menuCanvas').style.transform = `scale(${this.zoom})`;
        document.getElementById('btnResetZoom').innerHTML = `<i class="fas fa-expand"></i> ${Math.round(this.zoom * 100)}%`;
    }
    
    // Export Functions
    async exportAsPNG() {
        const canvas = document.getElementById('menuCanvas');
        const originalTransform = canvas.style.transform;
        canvas.style.transform = 'scale(1)';
        
        try {
            const dataUrl = await html2canvas(canvas, {
                scale: 2,
                backgroundColor: null,
                logging: false
            });
            
            const link = document.createElement('a');
            link.download = `${this.currentVersion.name}.png`;
            link.href = dataUrl.toDataURL();
            link.click();
        } catch (error) {
            console.error('Error exporting PNG:', error);
            alert('Erro ao exportar PNG');
        } finally {
            canvas.style.transform = originalTransform;
        }
    }
    
    async exportAsPDF() {
        const canvas = document.getElementById('menuCanvas');
        const originalTransform = canvas.style.transform;
        canvas.style.transform = 'scale(1)';
        
        try {
            const canvasImg = await html2canvas(canvas, {
                scale: 2,
                backgroundColor: null,
                logging: false
            });
            
            const imgData = canvasImg.toDataURL('image/png');
            const { jsPDF } = window.jspdf;
            const pdf = new jsPDF({
                orientation: 'portrait',
                unit: 'mm',
                format: 'a4'
            });
            
            const imgWidth = 210;
            const imgHeight = (canvasImg.height * imgWidth) / canvasImg.width;
            
            pdf.addImage(imgData, 'PNG', 0, 0, imgWidth, imgHeight);
            pdf.save(`${this.currentVersion.name}.pdf`);
        } catch (error) {
            console.error('Error exporting PDF:', error);
            alert('Erro ao exportar PDF');
        } finally {
            canvas.style.transform = originalTransform;
        }
    }
}

// Initialize application
let menuEditor;
document.addEventListener('DOMContentLoaded', () => {
    menuEditor = new MenuEditor();
});
