import '../css/gallery.css';

const GalleryGrid = () => {
  // Generate 24 placeholder items
  const items = Array.from({ length: 24 }, (_, i) => ({
    id: i,
    creator: 'Creator Name'
  }));

  return (
    <div className="gallery-container">
      <div className="masonry-grid">
        {items.map((item) => (
          <div key={item.id} className="masonry-item">
            <div className="image-placeholder"></div>
            <div className="image-overlay">
              <div className="image-info">
                <div className="creator-avatar"></div>
                <span className="creator-name">{item.creator}</span>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default GalleryGrid;