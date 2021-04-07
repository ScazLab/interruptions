<<<<<<< HEAD
import pandas as pd
import uuid
import csv

# generate a new valid code
def generate_code():
  return uuid.uuid4()

# register a code as a valid code
def save_code(
  item, # string code
  loc   # csv to save code to
):
  with open(loc, 'a', newline='', encoding='utf-8') as csvfile:
    csvwriter = csv.writer(csvfile)
    csvwriter.writerow([str(item)])

# check if code exists in csv of valid codes
def code_exists(
  item, # string code
  loc   # csv of valid codes
):
  with open(loc, 'r', newline='', encoding='utf-8') as csvfile:
    data = list(csv.reader(csvfile))
  return ([item] in data)

def main():
  # EXAMPLE USES
  # code_example = generate_code()
  # save_code(code_example, loc="codes.csv")
  # exists = code_exists("5604e359-66c9-46c2-ac78-feba39365e66", loc="codes.csv")
  # print("5604e359-66c9-46c2-ac78-feba39365e66" + " exists? : " + str(exists))

if __name__ == "__main__":
=======
import pandas as pd
import uuid
import csv

# generate a new valid code
def generate_code():
  return uuid.uuid4()

# register a code as a valid code
def save_code(
  item, # string code
  loc   # csv to save code to
):
  with open(loc, 'a', newline='', encoding='utf-8') as csvfile:
    csvwriter = csv.writer(csvfile)
    csvwriter.writerow([str(item)])

# check if code exists in csv of valid codes
def code_exists(
  item, # string code
  loc   # csv of valid codes
):
  with open(loc, 'r', newline='', encoding='utf-8') as csvfile:
    data = list(csv.reader(csvfile))
  return ([item] in data)

def main():
  # EXAMPLE USES
  # code_example = generate_code()
  # save_code(code_example, loc="codes.csv")
  # exists = code_exists("5604e359-66c9-46c2-ac78-feba39365e66", loc="codes.csv")
  # print("5604e359-66c9-46c2-ac78-feba39365e66" + " exists? : " + str(exists))

if __name__ == "__main__":
>>>>>>> 7e8e05dfae3224e308a48e66e5e32ad4ab361f71
  main()