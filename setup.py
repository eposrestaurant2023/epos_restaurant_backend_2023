from setuptools import setup, find_packages

with open("requirements.txt") as f:
	install_requires = f.read().strip().split("\n")

# get version from __version__ variable in epos_restaurant_2023/__init__.py
from epos_restaurant_2023 import __version__ as version

setup(
	name="epos_restaurant_2023",
	version=version,
	description="epos restaurant 2023 by ESTC ",
	author="Tes Pheakdey",
	author_email="pheakdey.micronet@gmail.com",
	packages=find_packages(),
	zip_safe=False,
	include_package_data=True,
	install_requires=install_requires
)
