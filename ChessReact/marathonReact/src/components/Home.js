import React from 'react'
import PlayersByCountry from './PlayersByCountry'

const Home = () => {
  return (
    <div className='w-full text-center'>
        <h1 className='text-4xl'>GET PLAYERS BY COUNTRY</h1>
        <PlayersByCountry/>
    </div>
  )
}

export default Home