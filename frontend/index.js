
const axios = require("axios")
const _ = require("lodash")

async function main() {
  const nums = [1, 2, 3, 4]
  console.log("Shuffled:", _.shuffle(nums))

  const res = await axios.get("https://jsonplaceholder.typicode.com/todos/1")
  console.log("API DATA:", res.data)
}

main()
