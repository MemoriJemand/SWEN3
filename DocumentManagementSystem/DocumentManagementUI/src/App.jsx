import { useState } from 'react'
import './App.css'

// install react router for navigation
// npm install react-router-dom

function Documents() {
    return (
        <>

        </>
    );
}

function App() {
    const [count, setCount] = useState(0)

  return (
    <>
          <h1>Document Management System</h1>

          <input id="search" type="text" /> {/* Change to use form & react input syntax */}
          <br /><br />

          <input id="document" type="file" /> {/* Change to use form & react input syntax */}
          <button onClick={ }></button>
          <br /><br />

          <div id="document_list"> {/* have it return table of all documents (that match search) along with update,delete & metadata buttons for each */}
          <Documents />
          </div>
    </>
  )
}

export default App
