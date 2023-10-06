import dayjs from 'dayjs'
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import 'dayjs/locale/ru'

import './index.css'

dayjs.locale('ru')

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
)
