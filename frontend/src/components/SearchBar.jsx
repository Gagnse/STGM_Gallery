import '../css/search.css';

const SearchBar = () => {
  return (
    <div className="search-container">
      <input 
        type="text" 
        className="search-bar" 
        placeholder="Search images..."
      />
    </div>
  );
};

export default SearchBar;