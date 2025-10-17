import '../css/sidebar.css';

const Sidebar = () => {
  const navItems = [
    { name: 'Images', active: true },
    { name: 'Textes', active: false },
    { name: 'Créer', active: false },
    { name: 'Fichier', active: false },
    { name: 'Collections', active: false }
  ];

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        <div className="logo">STGM x AI</div>
      </div>
      
      <nav className="sidebar-nav">
        {navItems.map((item) => (
          <a 
            key={item.name}
            href="#" 
            className={`nav-item ${item.active ? 'active' : ''}`}
          >
            <div className="nav-icon"></div>
            <span>{item.name}</span>
          </a>
        ))}
      </nav>
      
      <div className="sidebar-footer">
        <div className="user-profile">
          <div className="user-avatar"></div>
          <div className="user-info">
            <div className="user-name">Sébastien Gagnon</div>
            <div className="user-status">En ligne</div>
          </div>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;