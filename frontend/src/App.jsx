import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [apiStatus, setApiStatus] = useState('Checking...')
  const [weatherData, setWeatherData] = useState(null)

  useEffect(() => {
    const checkApi = async () => {
      try {
        const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'
        const response = await fetch(`${apiUrl}/weatherforecast`)
        
        if (response.ok) {
          const data = await response.json()
          setApiStatus('Connected ✓')
          setWeatherData(data)
        } else {
          setApiStatus('API Error')
        }
      } catch (error) {
        setApiStatus('Connection Failed')
        console.error('API connection error:', error)
      }
    }

    checkApi()
  }, [])

  return (
    <div className="App">
      <header className="App-header">
        <h1>🎨 Showcase Gallery</h1>
        <p>Development Environment</p>
      </header>

      <main className="App-main">
        <div className="status-card">
          <h2>System Status</h2>
          <div className="status-item">
            <span className="label">API Status:</span>
            <span className="value">{apiStatus}</span>
          </div>
          <div className="status-item">
            <span className="label">Environment:</span>
            <span className="value">Development</span>
          </div>
        </div>

        {weatherData && (
          <div className="status-card">
            <h2>API Test Data</h2>
            <p>Successfully fetched weather forecast from API:</p>
            <div className="weather-list">
              {weatherData.slice(0, 3).map((day, index) => (
                <div key={index} className="weather-item">
                  <strong>{day.date}</strong>: {day.temperatureC}°C - {day.summary}
                </div>
              ))}
            </div>
          </div>
        )}

        <div className="info-card">
          <h3>Ready for Development</h3>
          <p>✓ Frontend running on port 3000</p>
          <p>✓ Backend API on port 5000</p>
          <p>✓ PostgreSQL on port 5432</p>
          <p>✓ MinIO on ports 9000/9001</p>
        </div>
      </main>
    </div>
  )
}

export default App