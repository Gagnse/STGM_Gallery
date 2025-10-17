import Sidebar from './Sidebar';
import SearchBar from './SearchBar';
import GalleryGrid from './GalleryGrid';
import '../css/base.css';
import '../css/layout.css';

const LandingPage = () => {
  return (
    <>
      <Sidebar />
      
      <main className="main-content">
        <SearchBar />
        
        <div className="tab-header">
          <h1 className="tab-title">Top</h1>
        </div>
        
        <GalleryGrid />
      </main>
    </>
  );
};

export default LandingPage;